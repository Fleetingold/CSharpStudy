using AutoFixture;
using FluentAssertions;
using JWT.Algorithms;
using JWT.Extensions.AspNetCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace FlurlDemoTests
{
    public class FlurlAuthenticationTests
    {
        private static readonly Fixture _fixture = new Fixture();
        private static TestServer? _server;

        private HttpClient? _client;

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            var options = new JwtAuthenticationOptions
            {
                Keys = null,
                VerifySignature = true
            };
            _server = CreateServer(options);
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            _server?.Dispose();
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _client = _server?.CreateClient();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            _client?.Dispose();
        }

        [TestMethod]
        public async Task Request_Should_Return_Unauthorized_When_Token_Is_Empty()
        {
            if (_client != null)
            {
                // Arrange
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                    JwtAuthenticationDefaults.AuthenticationScheme,
                    String.Empty);

                // Act
                var response = await _client.GetAsync("https://example.com/");

                // Assert
                response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
            }
        }

        private static TestServer CreateServer(JwtAuthenticationOptions configureOptions)
        {
            var builder = new WebHostBuilder()
                .Configure(app =>
                {
                    app.UseAuthentication();

                    app.Use(async (HttpContext context, Func<Task> next) =>
                    {
                        var authenticationResult = await context.AuthenticateAsync();
                        if (authenticationResult.Succeeded)
                        {
                            context.Response.StatusCode = StatusCodes.Status200OK;
                            context.Response.ContentType = new ContentType("text/json").MediaType;

                            await context.Response.WriteAsync("Hello");
                        }
                        else
                        {
                            await context.ChallengeAsync();
                        }
                    });
                })
                .ConfigureServices(services =>
                {
                    services.AddAuthentication(options =>
                    {
                        // Prevents from System.InvalidOperationException: No authenticationScheme was specified, and there was no DefaultAuthenticateScheme found.
                        options.DefaultAuthenticateScheme = JwtAuthenticationDefaults.AuthenticationScheme;

                        // Prevents from System.InvalidOperationException: No authenticationScheme was specified, and there was no DefaultChallengeScheme found.
                        options.DefaultChallengeScheme = JwtAuthenticationDefaults.AuthenticationScheme;
                    })
                    .AddJwt(options =>
                    {
                        options.Keys = null;
                        options.VerifySignature = configureOptions.VerifySignature;
                    });
                    string ServerRsaPublicKey = "";
                    X509Certificate2 certificate = new X509Certificate2(Convert.FromBase64String(ServerRsaPublicKey));
                    IJwtAlgorithm algorithm = new RS256Algorithm(certificate);
                    services.AddSingleton<IAlgorithmFactory>(new DelegateAlgorithmFactory(algorithm));
                });

            return new TestServer(builder);
        }
    }
}
