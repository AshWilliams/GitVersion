using System;
using GitVersion;
using GitVersion.VersionCalculation;
using GitVersion.VersionFilters;
using GitVersionCore.Tests.Helpers;
using NUnit.Framework;
using Shouldly;
using GitVersionCore.Tests.Mocks;

namespace GitVersionCore.Tests.VersionFilters
{
    [TestFixture]
    public class ShaVersionFilterTests : TestBase
    {
        [Test]
        public void VerifyNullGuard()
        {
            Should.Throw<ArgumentNullException>(() => new ShaVersionFilter(null));
        }

        [Test]
        public void VerifyNullGuard2()
        {
            var commit = new MockCommit();
            var sut = new ShaVersionFilter(new[] { commit.Sha });

            Should.Throw<ArgumentNullException>(() => sut.Exclude(null, out _));
        }

        [Test]
        public void WhenShaMatchShouldExcludeWithReason()
        {
            var commit = new MockCommit();
            var context = new GitVersionContextBuilder().Build();
            var version = new BaseVersion(context, "dummy", false, new SemanticVersion(1), commit, string.Empty);
            var sut = new ShaVersionFilter(new[] { commit.Sha });

            sut.Exclude(version, out var reason).ShouldBeTrue();
            reason.ShouldNotBeNullOrWhiteSpace();
        }

        [Test]
        public void WhenShaMismatchShouldNotExclude()
        {
            var commit = new MockCommit();
            var context = new GitVersionContextBuilder().Build();
            var version = new BaseVersion(context, "dummy", false, new SemanticVersion(1), commit, string.Empty);
            var sut = new ShaVersionFilter(new[] { "mismatched" });

            sut.Exclude(version, out var reason).ShouldBeFalse();
            reason.ShouldBeNull();
        }

        [Test]
        public void ExcludeShouldAcceptVersionWithNullCommit()
        {
            var context = new GitVersionContextBuilder().Build();
            var version = new BaseVersion(context, "dummy", false, new SemanticVersion(1), null, string.Empty);
            var sut = new ShaVersionFilter(new[] { "mismatched" });

            sut.Exclude(version, out var reason).ShouldBeFalse();
            reason.ShouldBeNull();
        }
    }
}
