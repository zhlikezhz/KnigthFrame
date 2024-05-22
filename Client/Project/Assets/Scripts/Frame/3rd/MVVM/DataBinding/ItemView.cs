namespace Huge.MVVM.DataBinding
{
    public abstract class ItemView<TViewModel> : SubView
        where TViewModel : ViewModel
    {
        protected BindingSet bindSet;
        public virtual void ReplaceViewModel(TViewModel vm)
        {
            if (!IsDestroied())
            {
                bindSet?.Release();
                bindSet = BindingSet.Get();
                BindViewModel(bindSet, vm);
                bindSet.Build();
            }
        }

        public virtual void RemoveViewModel()
        {
            if (!IsDestroied())
            {
                UnbindViewModel();
            }
        }

        public virtual void BindViewModel(BindingSet bindSet, TViewModel vm)
        {

        }

        public virtual void UnbindViewModel()
        {
            bindSet?.Release();
            bindSet = null;
        }

        protected override void BeforeDestroy()
        {
            base.BeforeDestroy();
            bindSet?.Release();
        }
    }
}