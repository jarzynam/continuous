using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation.Runspaces;
using Continuous.Management.Common;
using FluentAssertions;
using NUnit.Framework;

namespace Continuous.Management.Tests
{
    [TestFixture]
    public class ScriptExecutorTests
    {
        private IScriptExecutor _executor;
        private ScriptsBoundle _scripts;
        
        [SetUp]
        public void SetUp()
        {
            _executor = new ScriptExecutor(GetType());
            _scripts = new ScriptsBoundle();  
        }

        [Test]
        public void Execute_Should_ReturnProperValue_WhenOk()
        {
            // arrange
            var name = "testName";

            var parameters = new List<CommandParameter>
            {
                new CommandParameter("name", name)
            };

            // act
            var result = _executor.Execute(_scripts.ValidScript, parameters);

            // assert
            (result.First().BaseObject as string).Should().Be(name);
        }

        [Test]
        public void Execute_Should_ThrowError_WhenOccur()
        {
            // arrange
            var name = "testName";

            var parameters = new List<CommandParameter>
            {
                new CommandParameter("name", name)
            };

            // act
            Action act = () => _executor.Execute(_scripts.InvalidScript, parameters);

            // assert
            act.ShouldThrow<InvalidOperationException>().WithMessage(name + Environment.NewLine);
        }

        [Test]
        public void Execute_Should_IgnoreErrorStream_WhenHasFlag()
        {
            // arrange
            var name = "testName";

            var parameters = new List<CommandParameter>
            {
                new CommandParameter("name", name)
            };

            // act
            Action act = () => _executor.Execute(_scripts.InvalidScript, parameters, true);

            // assert
            act.ShouldNotThrow();
        }

        
    }
}