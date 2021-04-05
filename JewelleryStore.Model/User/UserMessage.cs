namespace JewelleryStore.Model.User
{
    public class UserMessage
    {
        public int Rno { get; set; }

        public string Id { get; set; }

        public int DiscountPercentage { get; set; }

        public UserType Type { get; set; }
    }

    public enum UserType
    {
        NormalUser = 0,

        PrivilegedUser = 1
    }
}
