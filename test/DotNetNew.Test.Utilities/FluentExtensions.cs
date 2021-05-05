using System.Linq;
using FluentAssertions;
using FluentAssertions.Primitives;

namespace Steeltoe.DotNetNew.Test.Utilities
{
    public static class FluentExtensions
    {
        public static AndConstraint<StringAssertions> ContainSnippet(this StringAssertions assertion, string snippet)
        {
            var regex = RemoveWhitespace(snippet)
                .Replace("(", @"\(")
                .Replace(")", @"\)");
            var source = RemoveWhitespace(assertion.Subject);
            source.Should().MatchRegex(regex);
            return new AndConstraint<StringAssertions>(assertion);
        }

        private static string RemoveWhitespace(string s)
        {
            // return s.Replace(" ", "").
            return new(s.Where(c => !char.IsWhiteSpace(c)).ToArray());
        }
    }
}
