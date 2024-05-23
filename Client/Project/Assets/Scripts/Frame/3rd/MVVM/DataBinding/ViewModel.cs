namespace Huge.MVVM.DataBinding
{
    public abstract class ViewModel : ObservableObject
    {
        protected View view;
        public void SetView(View view)
        {
            this.view = view;
        }
    }
}