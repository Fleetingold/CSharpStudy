using AutoFixture;
using FluentAssertions;
using Flurl.Http;
using JWT.Algorithms;
using JWT.Extensions.AspNetCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Net.Http.Headers;
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
    [TestClass]
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
        public async Task Request_Should_Return_Ok_When_Token_Is_Valid()
        {
            if (_client != null)
            {
                string sTokenByAsymmetricAlgorithm = "eyJraWQiOiJDRkFFQUUyRDY1MEE2Q0E5ODYyNTc1REU1NDM3MUVBOTgwNjQzODQ5IiwidHlwIjoiSldUIiwiYWxnIjoiUlMyNTYifQ.eyJpc3MiOiJ0ZXN0IiwiZXhwIjoyMTQ3NDgzNjQ4LCJGaXJzdE5hbWUiOiJKZXN1cyIsIkFnZSI6MzN9.ZeGfWN3kBHZLiSh4jzzn6kx7F6lNu5OsowZW0Sv-_wpSgQO2_QXFUPLx23wm4J9rjMGQlSksEtCLd_X3iiBOBLbxAUWzdj59iJIAh485unZj12sBJ7KHDVsOMc6DcSJdwRo9S9yiJ_RJ57R-dn4uRdZTBXBZHrrmb35UjaAG6hFfu5d1Ap4ZjLxqDJGl0Wo4j5l6vR8HFpmiFHvqPQ4apjqkBGnitJ7oghbeRX0SIVNSkXbBDp3i9pC-hxzs2oHZC9ys0rJlfpxLls3MV4oQbQ7m6W9MrwwsdObJHI7PiTNfObLKdgySi6WkQS7rwXVz0DqRa8TXv8_USkvhsyGLMQ";

                // Arrange
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                    JwtAuthenticationDefaults.AuthenticationScheme, sTokenByAsymmetricAlgorithm);

                // Act
                var response = await _client.GetAsync("https://example.com/");

                // Assert
                response.StatusCode.Should().Be(HttpStatusCode.OK);
            }
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

        [TestMethod]
        public void Flurl_Request_Should_Return_Unauthorized_When_Token_Is_Empty()
        {
            Action action = () =>
            {
                "http://localhost:5000/cert/getinfo".PostJsonAsync(new
                {
                    OrderType = ""
                }).GetAwaiter().GetResult();
            };

            // Assert
            action.Should()
                  .Throw<FlurlHttpException>("because token is Empty.");
        }

        [TestMethod]
        public void Flurl_Request_Should_Return_200_When_Token_Is_Valid()
        {
            string sTokenByAsymmetricAlgorithm = "eyJraWQiOiJDRkFFQUUyRDY1MEE2Q0E5ODYyNTc1REU1NDM3MUVBOTgwNjQzODQ5IiwidHlwIjoiSldUIiwiYWxnIjoiUlMyNTYifQ.eyJpc3MiOiJ0ZXN0IiwiZXhwIjoyMTQ3NDgzNjQ4LCJGaXJzdE5hbWUiOiJKZXN1cyIsIkFnZSI6MzN9.ZeGfWN3kBHZLiSh4jzzn6kx7F6lNu5OsowZW0Sv-_wpSgQO2_QXFUPLx23wm4J9rjMGQlSksEtCLd_X3iiBOBLbxAUWzdj59iJIAh485unZj12sBJ7KHDVsOMc6DcSJdwRo9S9yiJ_RJ57R-dn4uRdZTBXBZHrrmb35UjaAG6hFfu5d1Ap4ZjLxqDJGl0Wo4j5l6vR8HFpmiFHvqPQ4apjqkBGnitJ7oghbeRX0SIVNSkXbBDp3i9pC-hxzs2oHZC9ys0rJlfpxLls3MV4oQbQ7m6W9MrwwsdObJHI7PiTNfObLKdgySi6WkQS7rwXVz0DqRa8TXv8_USkvhsyGLMQ";

            string header = $"{JwtAuthenticationDefaults.AuthenticationScheme} {sTokenByAsymmetricAlgorithm}";

            var response = "https://example.com/".WithHeader(HeaderNames.Authorization, header).GetAsync().GetAwaiter().GetResult();

            response.StatusCode.Should().Be(200);
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
                    string ServerRsaPublicKey = "MIIDfDCCAmSgAwIBAgIQQDCxkdjCQqmQsnSLtcHj3TANBgkqhkiG9w0BAQsFADA7MQswCQYDVQQGEwJ1czELMAkGA1UECBMCVVMxETAPBgNVBAoTCENlcnR0ZXN0MQwwCgYDVQQDEwNqd3QwHhcNMjAwMzIzMDI1NDAzWhcNMjMwMzIzMDMwNDAzWjA7MQswCQYDVQQGEwJ1czELMAkGA1UECBMCVVMxETAPBgNVBAoTCENlcnR0ZXN0MQwwCgYDVQQDEwNqd3QwggEiMA0GCSqGSIb3DQEBAQUAA4IBDwAwggEKAoIBAQC5hM+0cIjO0oLcxQPGdnSS0ZVJDSNVsPmtiXimSLhEIPczbZ35OSWa9PI+PRIztr/yjtwjTlCES4EjyEoJ8LYIQmGVLdYV5ULS/CyXVpgWpDdiSv6QOwB2qMv3mKiPcmaKxy+oo4zfihBqGkCC6QnspyvUFPZiWTx86Apw3u3WqBRE3HQ+PjMnjDSnWdPaAsb75ti61RU+9qYj3BwxDJR6xnAaYz1RSkxHOw4+Ty+/tNtObrZmTH7msVRpV7kMU1QgyD3Y2/JTTf3YUU0LCm1J+WJ0cMbVrILAvVlOQnRn3IlcI1LOL/e6XEyET5tVymv8S5EoJjGf2o8VnTsF3vttAgMBAAGjfDB6MA4GA1UdDwEB/wQEAwIFoDAJBgNVHRMEAjAAMB0GA1UdJQQWMBQGCCsGAQUFBwMBBggrBgEFBQcDAjAfBgNVHSMEGDAWgBTTMvXgytSFWwQk58CpxCpZAr5G1jAdBgNVHQ4EFgQU0zL14MrUhVsEJOfAqcQqWQK+RtYwDQYJKoZIhvcNAQELBQADggEBAK5vSwzh0x0pJm6njJX29rsd53ktyph+L90Enh0xzLFN0Ku9p+tM8E9TmKR+9ppdPqIEe4G/AuR1fHvmWenEw44M85Y/pBIPZDM2QVQngjg6iRQ42yD5hb/P4+UnvP9a5uI4Xc3f4NlJi3n54qBmdD5Hg52tNYgr8FKRoNzAoUCHelLk5PW0llF8Nc6cjJf0JfrSA1lVua488Dd34sPt798xM3IoISof1dqKslTypHP4BCyZ55SSfQJ+GrY7T9J3ct23BTrPnhhq0sPDogN4j258RmDriBGZmRcnrlmuBD5v+lvjYk0fISYNMfkrCQg5zae4d6BJIZVLY3gITGbaNoA=";
                    X509Certificate2 certificate = new X509Certificate2(Convert.FromBase64String(ServerRsaPublicKey));
                    IJwtAlgorithm algorithm = new RS256Algorithm(certificate);
                    services.AddSingleton<IAlgorithmFactory>(new DelegateAlgorithmFactory(algorithm));
                });

            return new TestServer(builder);
        }
    }
}
