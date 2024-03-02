using System;

namespace Huge.MVVM
{
    public sealed class ViewSettingAttribute : Attribute
    {
        public readonly Type Prefab;
        public readonly Type ViewModel;

        public ViewSettingAttribute(Type prefab)
        {
            Prefab = prefab;
        }
        
        public ViewSettingAttribute(Type prefab, Type viewModel)
        {
            Prefab = prefab;
            ViewModel = viewModel;
        }
    }
}