using System.Collections.Generic;

namespace Data.Repository.Caching {
	public abstract class CacheProvider<TCache> : ICacheProvider {
		private readonly int defaultCacheDurationInMinutes = 30;
		protected abstract TCache InitCache();

		protected TCache _cache;

		/// <summary>
		/// Expiration in minutes
		/// </summary>
		public int CacheDuration { get; set; }

		public CacheProvider() {
			CacheDuration = defaultCacheDurationInMinutes;
			_cache = InitCache();
		}
		public CacheProvider(int durationInMinutes) {
			CacheDuration = durationInMinutes;
			_cache = InitCache();
		}


		public abstract bool Get<T>(string key, out T value);

		public abstract void Set<T>(string key, T value);

		public abstract void Set<T>(string key, T value, int duration);

		public abstract void Clear(string key);

		public abstract IEnumerable<KeyValuePair<string, object>> GetAll();
	}
}