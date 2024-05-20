using UnityEngine;
using Huge.MVVM.DataBinding;

namespace Huge.MVVM
{
    public interface IViewGenerator
    {
        void BindGameObject(Transform transform);
        void BindViewModel(ViewModel viewModel);
        void UnbindViewModel();
    }
}
