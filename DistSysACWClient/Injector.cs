using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace DistSysACWClient
{
    public class Injector
    {
        private Dictionary<Type, object> _singletons;
        public Injector()
        {
            _singletons = new Dictionary<Type, object>();
        }

        public void Register<IType, Type>() where Type : class, IType 
        {
            if (_singletons.ContainsKey(typeof(IType)))
                throw new AlreadyRegisteredException($"{typeof(IType).Name} is already registered as a singleton.");

            _singletons[typeof(IType)] = typeof(Type);
        }

        /// <summary>
        /// Resolves a registered dependency.
        /// </summary>
        /// <typeparam name="IType">Must be the key that the dependency was registered with.</typeparam>
        /// <returns>The instance of the resolved dependency.</returns>
        public IType Resolve<IType>()
        {
            return (IType)Resolve(typeof(IType));
        }

        /// <summary>
        /// Resolves a registered dependency.
        /// </summary>
        /// <param name="type">Must be the key that the dependency was registered with.</param>
        /// <returns>The instance of the resolved dependency.</returns>
        public object Resolve(Type type)
        {
            if (_singletons.ContainsKey(type))
            {
                if (!typeof(Type).IsAssignableFrom(_singletons[type].GetType()))
                    return _singletons[type];

                return _singletons[type] = Construct(_singletons[type] as Type);
            }

            throw new FailedToResolveTypeException($"Failed to resolve the type {type.FullName}.");
        }

        /// <summary>
        /// Constructs a concrete type by resolving all of the parameters in it's constructor. It will look for the constructor that has the most parameters but can still be resolved.
        /// </summary>
        /// <typeparam name="T">A concrete type. Should be a class...</typeparam>
        /// <returns>The constructed object.</returns>
        public T Construct<T>() where T : class
        {
            return Construct(typeof(T)) as T;
        }

        /// <summary>
        /// Constructs a concrete type by resolving all of the parameters in it's constructor. It will look for the constructor that has the most parameters but can still be resolved.
        /// </summary>
        /// <param name="type">A concrete type. Should be a class...</param>
        /// <returns>The constructed object.</returns>
        public object Construct(Type type)
        {
            var constructorInfos = new List<ConstructorInfo>(type.GetConstructors());

            List<object> parameters = new List<object>();
            foreach (var constructorInfo in constructorInfos.OrderByDescending(c => c.GetParameters().Length))
            {
                try
                {
                    foreach (var constructorParameter in constructorInfo.GetParameters())
                        parameters.Add(Resolve(constructorParameter.ParameterType));
                }
                catch (FailedToResolveTypeException e)
                {
                    continue;
                }
            }

            return Activator.CreateInstance(type, parameters.ToArray());
        }
    }
}
