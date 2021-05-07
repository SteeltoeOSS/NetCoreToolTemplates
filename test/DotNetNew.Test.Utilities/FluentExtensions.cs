using System.Text.RegularExpressions;
using FluentAssertions;
using FluentAssertions.Primitives;

namespace Steeltoe.DotNetNew.Test.Utilities
{
    public static class FluentExtensions
    {
        public static AndConstraint<StringAssertions> ContainSnippet(this StringAssertions assertion, string snippet)
        {
            var regex = Regex.Replace(snippet, @"\s+", @"\s+")
                .Replace("(", @"\(").Replace(")", @"\)")
                .Replace("[", @"\[").Replace("]", @"\]")
                .Replace("|", @"\|")
                .Replace(".", @"\s*\.\s*");
            assertion.Subject.Should().MatchRegex(regex);
            return new AndConstraint<StringAssertions>(assertion);
        }
    }
}
