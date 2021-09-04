namespace StaticDataViewModel
{
    public class Static
    {
        public static Static Inst
        {
            get;
            private set;
        } = new Static();

        public Static()
        {

        }

        public ViewModel.Collection Collection { get; private set; } = new ViewModel.Collection();
    }
}
