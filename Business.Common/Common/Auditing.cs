using System;
using System.Collections.Concurrent;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Business.Common {
	public static class Auditing {
		private static ConcurrentDictionary<RuntimeTypeHandle, PropertyInfo> KeyPropertiesCache = new ConcurrentDictionary<RuntimeTypeHandle, PropertyInfo>();
		
		/// <summary>
		/// Sets IsActive, IsDeleted, CreatedDate and UpdatedDate for the proper circumstances.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="entity"></param>
		/// <param name="isNew">Allows new items to be treated as such even if they do not have a default value applied to their key.</param>
		public static bool SetAuditFields<T>(T entity, bool isNew = false) where T : class {
			if (!isNew) {
				isNew = isEntityNew(entity);	
			}
			if (isNew) {
				setNonOverrideableProperty<T, DateTime>("CreatedDate", entity, DateTime.Now);
				setNonOverrideableProperty<T, bool>("IsActive", entity, true);
				setNonOverrideableProperty<T, bool>("IsDeleted", entity, false);
			} else {
				setOverrideableProperty<T, DateTime>("UpdatedDate", entity, DateTime.Now);
			}
			return isNew;
		}

		/// <summary>
		/// Sets IsActive, IsDeleted, CreatedById, CreatedDate, UpdatedDate and UpdatedById for the proper circumstances.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="entity"></param>
		/// <param name="currentUser"></param>
		/// <param name="isNew">Allows new items to be treated as such even if they do not have a default value applied to their key.</param>
		public static bool SetAuditFields<T>(T entity, long currentUserId, bool isNew = false) where T : class {
			isNew = SetAuditFields<T>(entity, isNew);
			if (currentUserId == null || currentUserId == 0) {
				return isNew;
			}
			if (isNew) {
				setNonOverrideableProperty<T, long>("CreatedById", entity, currentUserId);
			} else {
				setOverrideableProperty<T, long>("UpdatedById", entity, currentUserId);
			}
			return isNew;
		}

		private static bool isEntityNew<T>(T entity) where T : class {
			PropertyInfo keyProperty = getKeyProperty<T>() ;
			if (keyProperty != null) {
				var defaultValue = getDefault(keyProperty.PropertyType);
				var value = keyProperty.GetValue(entity);
				if (value.Equals(defaultValue)) {
					return true;
				}
			}
			return false;
		}

		private static object getDefault(Type type) {
			// If no Type was supplied, if the Type was a reference type, or if the Type was a System.Void, return null
			if (type == null || !type.IsValueType || type == typeof(void))
				return null;

			// If the supplied Type has generic parameters, its default value cannot be determined
			if (type.ContainsGenericParameters)
				throw new ArgumentException(
					"{" + MethodInfo.GetCurrentMethod() + "} Error:\n\nThe supplied value type <" + type +
					"> contains generic parameters, so the default value cannot be retrieved");

			// If the Type is a primitive type, or if it is another publicly-visible value type (i.e. struct), return a 
			//default instance of the value type
			if (type.IsPrimitive || !type.IsNotPublic) {
				try {
					return Activator.CreateInstance(type);
				} catch (Exception e) {
					throw new ArgumentException(
						"{" + MethodInfo.GetCurrentMethod() + "} Error:\n\nThe Activator.CreateInstance method could not " +
						"create a default instance of the supplied value type <" + type +
						"> (Inner Exception message: \"" + e.Message + "\")", e);
				}
			}

			throw new ArgumentException("{" + MethodInfo.GetCurrentMethod() + "} Error:\n\nThe supplied value type <" + type + "> is not a publicly-visible type, so the default value cannot be retrieved");
		}

		private static PropertyInfo getKeyProperty<T>() where T : class {
			var keyProperty = KeyPropertiesCache.GetOrAdd(typeof(T).TypeHandle, findKeyProperty<T>());
			return keyProperty;
		}

		private static PropertyInfo findKeyProperty<T>() where T : class {
			PropertyInfo keyProperty = null;
			var properties = typeof(T).GetProperties();
			foreach (PropertyInfo property in properties) {
				var attribute = Attribute.GetCustomAttribute(property, typeof(KeyAttribute)) as KeyAttribute;
				if (attribute != null) {
					keyProperty = property;
					break;
				}
			}
			return keyProperty;
		}

		private static void setNonOverrideableProperty<T, R>(string propertyName, T entity, R propertyValue) where T : class where R : struct {
			var property = typeof(T).GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance);
			if (property != null && property.CanWrite) {
				var obj = property.GetValue(entity);
				if (obj != null) {
					R value = (R)obj;
					R defaultValue = default(R);
					if (value.Equals(null) || value.Equals(defaultValue)) {
						property.SetValue(entity, propertyValue, null);
					}
				} else { 
					property.SetValue(entity, propertyValue, null);
				}
			}
		}
		private static void setOverrideableProperty<T, R>(string propertyName, T entity, R propertyValue)
			where T : class
			where R : struct {
			var property = typeof(T).GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance);
			if (property != null && property.CanWrite) {
				property.SetValue(entity, propertyValue, null);
			}
		}
	}
}