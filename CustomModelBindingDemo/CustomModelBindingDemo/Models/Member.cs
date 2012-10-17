using System.Web.Mvc;
using CustomModelBindingDemo.Infrastructure;

namespace CustomModelBindingDemo.Models
{
    //[ModelBinder(typeof(MemberModelBinder))]
    public class Member
    {
        public Member(
            string firstName,
            string lastName)
        {
            this.FirstName = firstName;
            this.LastName = lastName;
            this.IsApproved = false;
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsApproved{ get; set; }
    }
}