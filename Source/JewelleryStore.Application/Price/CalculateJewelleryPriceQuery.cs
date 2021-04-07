using FluentValidation;
using JewelleryStore.Application.Common;
using JewelleryStore.Application.User;
using JewelleryStore.Model.Jewellery;
using JewelleryStore.Model.Resources;
using JewelleryStore.Model.User;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace JewelleryStore.Application.Price
{
    public class CalculateJewelleryPriceQuery : JewelleryMessage, IRequest<double> { }

    public class CalculateJewelleryPriceQueryHandler : IRequestHandler<CalculateJewelleryPriceQuery, double>
    {
        private readonly IUserDataAccess _userDataAccess;
        private readonly AbstractValidator<JewelleryMessage> _messageValidator;
        private readonly IUserContext _userContext;

        public CalculateJewelleryPriceQueryHandler(IUserContext userContext, IUserDataAccess userDataAccess, AbstractValidator<JewelleryMessage> messageValidator)
        {
            _userDataAccess = userDataAccess;
            _messageValidator = messageValidator;
            _userContext = userContext;
        }

        public async Task<double> Handle(CalculateJewelleryPriceQuery request, CancellationToken cancellationToken)
        {
            await ValidateJewelleryMessageRequest(request, cancellationToken);
            return await GetTotalPrice(request);
        }

        private async Task ValidateJewelleryMessageRequest(CalculateJewelleryPriceQuery request, CancellationToken cancellationToken)
        {
            var result = await _messageValidator.ValidateAsync(request, cancellationToken);

            if (!result.IsValid)
            {
                throw new ValidationException(string.Format(ErrorMessage.ValidationError, CommonMessage.Jewellery), result.Errors);
            }
        }

        private async Task<double> GetTotalPrice(CalculateJewelleryPriceQuery request)
        {
            var userMessage = await _userDataAccess.DetailsAsync(_userContext.UserRno);

            var totalPrice = request.Price * request.Weight;

            if (userMessage.Type == UserType.PrivilegedUser)
            {
                var discount = totalPrice * ((double)userMessage.DiscountPercentage / 100);
                return totalPrice - discount;
            }

            return totalPrice;
        }
    }
}
