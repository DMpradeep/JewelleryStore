using FluentValidation;
using JewelleryStore.Application.User;
using JewelleryStore.Model.Jewellery;
using JewelleryStore.Model.Resources;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace JewelleryStore.Application.Price
{
    public class CalculateJewelleryPriceQuery : JewelleryMessage, IRequest<double>
    {
        public int UserRno { get; set; }
    }

    public class CalculateJewelleryPriceQueryHandler : IRequestHandler<CalculateJewelleryPriceQuery, double>
    {
        private readonly IUserDataAccess _userDataAccess;
        private readonly AbstractValidator<JewelleryMessage> _messageValidator;

        public CalculateJewelleryPriceQueryHandler(IUserDataAccess userDataAccess, AbstractValidator<JewelleryMessage> messageValidator)
        {
            _userDataAccess = userDataAccess;
            _messageValidator = messageValidator;
        }

        public async Task<double> Handle(CalculateJewelleryPriceQuery request, CancellationToken cancellationToken)
        {
            await ValidateJewelleryMessageRequest(request, cancellationToken);

            var userMessage = await _userDataAccess.DetailsAsync(request.UserRno);

            var totalPrice = request.Price * request.Weight;
            var discount = totalPrice * ((double)userMessage.DiscountPercentage / 100);

            return totalPrice - discount;
        }

        private async Task ValidateJewelleryMessageRequest(CalculateJewelleryPriceQuery request, CancellationToken cancellationToken)
        {
            var result = await _messageValidator.ValidateAsync(request, cancellationToken);

            if (!result.IsValid)
            {
                throw new ValidationException(string.Format(ErrorMessage.ValidationError, CommonMessage.Jewellery), result.Errors);
            }
        }
    }
}
