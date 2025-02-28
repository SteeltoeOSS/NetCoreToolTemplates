using FluentAssertions;
using FluentAssertions.Primitives;

namespace Steeltoe.NetCoreTool.Template.WebApi.Test.Assertions
{
    public static class FluentExtensions
    {
        public static AndConstraint<StringAssertions> ContainSnippet(this StringAssertions assertion, string snippet)
        {
            assertion.Subject.Should().Contain(snippet, Exactly.Once());
            return new AndConstraint<StringAssertions>(assertion);
        }

        public static AndConstraint<StringAssertions> ContainRegexSnippet(this StringAssertions assertion, string snippet)
        {
            assertion.Subject.Should().MatchRegex(snippet);
            return new AndConstraint<StringAssertions>(assertion);
        }

        public static AndConstraint<StringAssertions> NotContainRegexSnippet(this StringAssertions assertion, string snippet)
        {
            assertion.Subject.Should().NotMatchRegex(snippet);
            return new AndConstraint<StringAssertions>(assertion);
        }
    }
}
