using CustomModelBindingDemo.Models;

namespace CustomModelBindingDemo.ViewModels
{
    public class MemberViewModel
    {
        public Member Member { get; set; }

        public MemberViewModel()
        {
            Member = new Member(null, null);
        }
    }
}