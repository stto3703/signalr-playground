using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using Push.Core;
using System.Linq;

namespace Tests
{
	[TestFixture]
	public class TestJsonPatches
	{
		[TestCase("{a:1, b:2, c:3}",
			"{a:1, b:2}",
			ExpectedResult = "[{\"op\":\"remove\",\"path\":\"/c\",\"value\":null}]",
			TestName = "JsonPatch remove works for a simple value")]

		[TestCase("{a:1, b:2, c:{d:1,e:2}}",
			"{a:1, b:2}",
			ExpectedResult = "[{\"op\":\"remove\",\"path\":\"/c\",\"value\":null}]",
			TestName = "JsonPatch remove works for a complex value")]

		[TestCase("{a:1, b:2}",
			"{a:1, b:2, c:3}",
			ExpectedResult = "[{\"op\":\"add\",\"path\":\"/c\",\"value\":3}]",
			TestName = "JsonPatch add works for a simple value")]

		[TestCase("{a:1, b:2}",
			"{a:1, b:2, c:{d:1,e:2}}",
			ExpectedResult = "[{\"op\":\"add\",\"path\":\"/c\",\"value\":{\"d\":1,\"e\":2}}]",
			TestName = "JsonPatch add works for a complex value")]

		[TestCase("{a:1, b:2, c:3}",
			"{a:1, b:2, c:4}",
			ExpectedResult = "[{\"op\":\"replace\",\"path\":\"/c\",\"value\":4}]",
			TestName = "JsonPatch replace works for int")]

		[TestCase("{a:1, b:2, c:\"foo\"}",
			"{a:1, b:2, c:\"bar\"}",
			ExpectedResult = "[{\"op\":\"replace\",\"path\":\"/c\",\"value\":\"bar\"}]",
			TestName = "JsonPatch replace works for string")]

		[TestCase("{a:1, b:2, c:{foo:1}}",
			"{a:1, b:2, c:{bar:2}}",
			ExpectedResult = "[{\"op\":\"remove\",\"path\":\"/c/foo\",\"value\":null},{\"op\":\"add\",\"path\":\"/c/bar\",\"value\":2}]",
			TestName = "JsonPatch replace works for object")]

		[TestCase("{a:1, b:2, c:3}",
			"{c:3, b:2, a:1}",
			ExpectedResult = "[]",
			TestName = "JsonPatch order does not matter")]

		[TestCase("{a:{b:{c:{d:1}}}}",
			"{a:{b:{d:{c:1}}}}",
			ExpectedResult = "[{\"op\":\"remove\",\"path\":\"/a/b/c\",\"value\":null},{\"op\":\"add\",\"path\":\"/a/b/d\",\"value\":{\"c\":1}}]",
			TestName = "JsonPatch handles deep nesting")]

		[TestCase("[1,2,3,4]",
			"[5,6,7]",
			ExpectedResult = "[{\"op\":\"replace\",\"path\":\"\",\"value\":[5,6,7]}]",
			TestName = "JsonPatch handles a simple array and replaces it")]

		[TestCase("{a:[1,2,3,4]}",
			"{a:[5,6,7]}",
			ExpectedResult = "[{\"op\":\"replace\",\"path\":\"/a\",\"value\":[5,6,7]}]",
			TestName = "JsonPatch handles a simple array under a property and replaces it")]

		[TestCase("{a:[1,2,3,4]}",
			"{a:[1,2,3,4]}",
			ExpectedResult = "[]",
			TestName = "JsonPatch handles same array")]

		public string JsonPatchesWorks(string leftString, string rightString)
		{
			var left = JToken.Parse(leftString);
			var right = JToken.Parse(rightString);

			var patches = JsonPatch.CalculatePatch(left, right).ToList();
			var pts = JsonConvert.SerializeObject(patches);
			System.Diagnostics.Trace.WriteLine(pts);
			return pts;
		}
	}
}
