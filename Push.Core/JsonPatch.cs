using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Push.Core
{
	public class JsonPatch
	{
		[JsonProperty("op")]
		public string Operation { get; set; } // add, remove, replace

		[JsonProperty("path")]
		public string Path { get; set; }

		[JsonProperty("value")]
		public JToken Value { get; set; }

		private JsonPatch()
		{
		}

		public static string Extend(string path, string extension)
		{
			// TODO: JSON property name needs escaping for path ??
			return path + "/" + extension;
		}

		private static JsonPatch Build(string op, string path, string key, JToken value)
		{
			return string.IsNullOrEmpty(key) 
				? new JsonPatch {Operation = op, Path = path, Value = value} 
				: new JsonPatch {Operation = op, Path = Extend(path, key), Value = value};
		}

		public static JsonPatch Add(string path, string key, JToken value)
		{
			return Build("add", path, key, value);
		}

		public static JsonPatch Remove(string path, string key)
		{
			return Build("remove", path, key, null);
		}

		public static JsonPatch Replace(string path, string key, JToken value)
		{
			return Build("replace", path, key, value);
		}

		public static IEnumerable<JsonPatch> CalculatePatch(JToken left, JToken right, string path = "")
		{
			if (left.Type != right.Type)
			{
				yield return JsonPatch.Replace(path, "", right);
				yield break;
			}

			if (left.Type == JTokenType.Array)
			{
				if (left.Children().SequenceEqual(right.Children())) // TODO: Need a DEEP EQUALS HERE
					yield break;

				// No array insert or delete operators in jpatch (yet?)
				yield return JsonPatch.Replace(path, "", right);
				yield break;
			}

			if (left.Type == JTokenType.Object)
			{
				var lprops = ((IDictionary<string, JToken>) left).OrderBy(p => p.Key);
				var rprops = ((IDictionary<string, JToken>) right).OrderBy(p => p.Key);

				foreach (var removed in lprops.Except(rprops, MatchesKey.Instance))
				{
					yield return JsonPatch.Remove(path, removed.Key);
				}

				foreach (var added in rprops.Except(lprops, MatchesKey.Instance))
				{
					yield return JsonPatch.Add(path, added.Key, added.Value);
				}

				var matchedKeys = lprops.Select(x => x.Key).Intersect(rprops.Select(y => y.Key));
				var zipped = matchedKeys.Select(k => new {key = k, left = left[k], right = right[k]});

				foreach (var match in zipped)
				{
					string newPath = path + "/" + match.key;
					foreach (var patch in CalculatePatch(match.left, match.right, newPath))
						yield return patch;
				}
				yield break;
			}
			else
			{
				// Two values, same type, not JObject so no properties

				if (left.ToString() == right.ToString())
					yield break;
				else
					yield return JsonPatch.Replace(path, "", right);
			}
		}

		private class MatchesKey : IEqualityComparer<KeyValuePair<string, JToken>>
		{
			public static readonly MatchesKey Instance = new MatchesKey();

			public bool Equals(KeyValuePair<string, JToken> x, KeyValuePair<string, JToken> y)
			{
				return x.Key.Equals(y.Key);
			}

			public int GetHashCode(KeyValuePair<string, JToken> obj)
			{
				return obj.Key.GetHashCode();
			}
		}
	}
}