using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace WorldOfZero.Common.MVVM.ViewModels
{
    /// <summary>
    /// Base class to derive ViewModel implementations that encapsulate an Entity type.
    /// </summary>
    /// <typeparam name="T">The entity type.</typeparam>
    public abstract class BaseViewModel : IViewModel
    {
        private readonly IDictionary<string, Action> _propertyNotifications;

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewModelBase{T}"/> class.
        /// </summary>
        /// <param name="entityType">An instance of the entity type to encapsulate.</param>
        protected BaseViewModel()
        {
            _propertyNotifications = ReflectTypeProperties();
        }

        #region INotifyPropertyChanged implementation
        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Notifies listener that the value of the specified property has changed.
        /// </summary>
        /// <param name="propertyName">The name of the property to notify about.</param>
        public void NotifyPropertyChanged(string propertyName)
        {
            Action notify;
            _propertyNotifications.TryGetValue(propertyName, out notify);
            if (notify != null) notify();
        }

        /// <summary>
        /// Notifies listener that the value of the specified property has changed.
        /// </summary>
        /// <typeparam name="TProperty">The type of the property (inferred).</typeparam>
        /// <param name="property">An expression that selects a property, like <c>() => PropertyName</c>.</param>
        public void NotifyPropertyChanged<TProperty>(Expression<Func<TProperty>> property)
        {
            NotifyPropertyChanged(PropertyName(property));
        }

        private void NotifyPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null) PropertyChanged(sender, e);
        }

        /// <summary>
        /// Loads the names of all properties of the most derived type into a
        /// Dictionary where each entry (property name) points to a delegate that
        /// calls <see cref="NotifyPropertyChanged"/> for the corresponding property.
        /// </summary>
        private IDictionary<string, Action> ReflectTypeProperties()
        {
            var viewModelProperties = GetType().GetProperties().Where(p => p.CanWrite); // uses reflection (slow)

            return viewModelProperties
                   .Select(property => new KeyValuePair<string, Action>(property.Name,
                                 () => NotifyPropertyChanged(this, new PropertyChangedEventArgs(property.Name))))
                   .ToDictionary(kv => kv.Key, kv => kv.Value);
        }

        /// <summary>
        /// Returns the name of a property in a LINQ Expression such as '<code>() => Property</code>'.
        /// Used for strongly-typed INotifyPropertyChanged implementation.
        /// </summary>
        protected static string PropertyName<TProperty>(Expression<Func<TProperty>> property)
        {
            var lambda = (LambdaExpression)property;
            MemberExpression memberExpression;

            var body = lambda.Body as UnaryExpression;
            if (body == null)
                memberExpression = (MemberExpression)lambda.Body;
            else
            {
                var unaryExpression = body;
                memberExpression = (MemberExpression)unaryExpression.Operand;
            }

            return memberExpression.Member.Name;
        }
        #endregion
    }
}
