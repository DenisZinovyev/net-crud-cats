namespace DSR.CrudCats.Users.CreateResponse
{
    public class User
    {
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public User(string email, string firstName, string lastName) =>
            (Email, FirstName, LastName) = (email, firstName, lastName);
    }
}
