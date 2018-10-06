using System;
using System.Collections.Generic;
using FluentAssertions;
using Xunit;

namespace UrlMapper.Tests
{
    public class BuilderTests
    {
        [Theory(DisplayName = "Builder สามารถทำการแยกข้อมูลจาก url ที่ match กันได้ถูกต้อง")]
        [InlineData("https://mana.com/linkto/{link-id}", "https://mana.com/linkto/A2348", "{link-id}", "A2348", true)]
        [InlineData("http://google.com/?s={keyword}", "http://google.com/?s=Xamarin", "{keyword}", "Xamarin", true)]
        [InlineData("https://mana.com/app/{app-id}/services/{service-id}", "https://mana.com/app/di394/services/878", "{app-id},{service-id}", "di394,878", true)]
        [InlineData("https://mana.com/nana/{app/-id}/services/{service-id}", "https://mana.com/nana/di394/services/services/878", "{app/-id},{service-id}", "di394,services/878", true)]
        public void BuilderCanWorkCorrectly(string pattern, string url, string keys, string values, bool expectedMatched)
        {
            var builder = FirstRound.Builder;
            var sut = builder.Parse(pattern);

            var expectedDic = new Dictionary<string, string>();
            var expectedKeys = keys.Split(',');
            var expectedValues = values.Split(',');

            for (int elementIndex = 0; elementIndex < expectedKeys.Length; elementIndex++)
                expectedDic.Add(expectedKeys[elementIndex], expectedValues[elementIndex]);

            var actual = new Dictionary<string, string>();
            var isMatch = sut.IsMatched(url);
            isMatch.Should().Be(expectedMatched);
            if (isMatch)
            {
                sut.ExtractVariables(url, actual);
                actual.Should().NotBeNull().And.BeEquivalentTo(expectedDic);
            }
        }
    }
}