using System.Text.RegularExpressions;
using FluentAssertions;
using FluentAssertions.Primitives;

namespace Steeltoe.DotNetNew.Test.Utilities.Assertions
{
    public static class FluentExtensions
    {
        public static AndConstraint<StringAssertions> ContainSnippet(this StringAssertions assertion, string snippet)
        {
            assertion.Subject.Should().MatchRegex(RegexForSnippet(snippet));
            return new AndConstraint<StringAssertions>(assertion);
        }

        public static AndConstraint<StringAssertions> NotContainSnippet(this StringAssertions assertion, string snippet)
        {
            assertion.Subject.Should().NotMatchRegex(RegexForSnippet(snippet));
            return new AndConstraint<StringAssertions>(assertion);
        }

        private static string RegexForSnippet(string snippet)
        {
            var regex = snippet
                .Replace("(", @"\(").Replace(")", @"\)")
                .Replace("[", @"\[").Replace("]", @"\]")
                .Replace("|", @"\|")
                .Replace("+", @"\+")
                .Replace("$", @"\$")
                .Replace(".", @"\s*\.\s*");
            return Regex.Replace(regex, @"\s+", @"\s+");
        }
    }
}
