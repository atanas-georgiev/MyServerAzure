﻿namespace Microsoft.AspNetCore.Authentication
{
    public class AzureAdB2COptions
    {
        public const string PolicyAuthenticationProperty = "Policy";

        public string CallbackPath { get; set; }

        public string ClientId { get; set; }

        public string DefaultPolicy => this.SignUpSignInPolicyId;

        public string Domain { get; set; }

        public string EditProfilePolicyId { get; set; }

        public string Instance { get; set; }

        public string ResetPasswordPolicyId { get; set; }

        public string SignUpSignInPolicyId { get; set; }
    }
}