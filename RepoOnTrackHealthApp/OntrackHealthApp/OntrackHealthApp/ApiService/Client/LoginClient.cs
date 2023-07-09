using Newtonsoft.Json.Linq;
using OntrackHealthApp.ApiHelper;
using OntrackHealthApp.ApiHelper.Client;
using OntrackHealthApp.ApiHelper.Response;
using OntrackHealthApp.ApiService.Model;
using OntrackHealthApp.ApiService.Response;
using OntrackHealthApp.ApiService.ViewModel;
using OntrackHealthApp.ViewModel;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OntrackHealthApp.ApiService.Client
{
    public class LoginClient : ClientBase, ILoginClient
    {
        private const string TokenUri = "Token";
        private const string ResetPasswordUri = "api/Account/LoginUserResetPasswordFirst";
        private const string UserIdentityUri = "api/Account/LoginUserIdentity";
        private readonly string UserPracticeUri = "api/Account/LoginUserPractice";
        private readonly string ApplicationSettingUri = "api/Account/ApplicationSetting";
        private readonly string ResetPasswordFirstUri = "api/Account/ResetPasswordFirst";
        private readonly string ChangePasswordUri = "api/Account/ChangePassword";

        public LoginClient(IApiClient iApiClient) : base(iApiClient)
        {
        }

        public async Task<TokenResponse> Login(string email, string password)
        {
            var response = await _iApiClient.PostFormEncodedContent(TokenUri, "grant_type".AsPair("password"), "username".AsPair(email), "password".AsPair(password));
            var tokenResponse = await CreateJsonResponse<TokenResponse>(response);
            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await DecodeContent<dynamic>(response);
                string error = Convert.ToString(errorContent["error"]);
                string error_description = Convert.ToString(errorContent["error_description"]);
                tokenResponse.ErrorState = new ErrorStateResponse
                {
                    Message = error_description,
                    ModelState = new Dictionary<string, string[]>
                    {
                        {error, new string[] { error_description }}
                    }
                };
                return tokenResponse;
            }

            //var tokenData = await DecodeContent<dynamic>(response);
            //tokenResponse.Data = tokenData["access_token"];
            var result = await response.Content.ReadAsStringAsync();
            var jObject = JObject.Parse(result);
            tokenResponse.Data = jObject.GetValue("access_token").ToString();
            return tokenResponse;
        }

        public async Task<ResetPasswordResponse> LoginUserResetPasswordFirst()
        {
            return await GetJsonDecodedContent<ResetPasswordResponse, ResetPasswordViewModel>(ResetPasswordUri);
        }

        public async Task<UserIdentityResponse> LoginUserIdentity()
        {
            return await GetJsonDecodedContent<UserIdentityResponse, UserIdentityModel>(UserIdentityUri);
        }

        public async Task<UserPracticeListResponse> LoginUserPractices(string email)
        {
            //UserPracticeUri += "/" + email;
            var userPracticeUri = UserPracticeUri + "?email=" + email;
            return await GetJsonListDecodedContent<UserPracticeListResponse, List<UserPracticeViewModel>>(userPracticeUri);
        }

        public async Task<ResetPasswordPostResponse> ResetPasswordFirst(ResetPasswordViewModel viewModel)
        {
            var resetPasswordPostResponse = await PostJsonEncodedContentAsync<ResetPasswordPostResponse, ResetPasswordViewModel>(ResetPasswordFirstUri, viewModel);
            return resetPasswordPostResponse;
        }

        public async Task<ChangePasswordResponse> ChangePassword(ChangePasswordViewModel viewModel)
        {
            var changePasswordResponse = await PostJsonEncodedContentAsync<ChangePasswordResponse, ChangePasswordViewModel>(ChangePasswordUri, viewModel);
            return changePasswordResponse;
        }

        public async Task<ApplicationSettingResponse> GetApplicationSetting()
        {
            return await GetJsonDecodedContent<ApplicationSettingResponse, ApplicationSettingModel>(ApplicationSettingUri);
        }
    }

    public interface ILoginClient
    {
        Task<TokenResponse> Login(string email, string password);
        Task<ResetPasswordResponse> LoginUserResetPasswordFirst();
        Task<UserIdentityResponse> LoginUserIdentity();
        Task<UserPracticeListResponse> LoginUserPractices(string email);
        Task<ResetPasswordPostResponse> ResetPasswordFirst(ResetPasswordViewModel viewModel);
        Task<ChangePasswordResponse> ChangePassword(ChangePasswordViewModel viewModel);
        Task<ApplicationSettingResponse> GetApplicationSetting();
    }
}
