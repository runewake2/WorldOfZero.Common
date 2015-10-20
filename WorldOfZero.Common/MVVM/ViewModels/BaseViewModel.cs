using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace WorldOfZero.Common.MVVM.ViewModels
{
    /// <summary>
    /// Base class to derive ViewModel implementations.
    /// </summary>
    public abstract class BaseViewModel : IViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ViewModelBase{T}"/> class.
        /// </summary>
        /// <param name="entityType">An instance of the entity type to encapsulate.</param>
        protected BaseViewModel()
        {
        }

        #region INotifyPropertyChanged implementation
        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Notifies listener that the value of the specified property has changed.
        /// 
        /// Will automatically imply the property name if called from the changed property, alternatively use nameof to pass the name manually.
        /// </summary>
        /// <param name="propertyName">The name of the property to notify about.</param>
        public void NotifyPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
