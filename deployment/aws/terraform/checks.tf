check "existing_vpc" {
  assert {
    condition     = var.create_vpc || (var.vpc_id != "" && length(var.public_subnet_ids) >= 2)
    error_message = "When create_vpc is false, set vpc_id and at least two public_subnet_ids."
  }
}

check "github_oidc" {
  assert {
    condition     = var.create_github_oidc_provider || var.github_oidc_provider_arn != ""
    error_message = "Set github_oidc_provider_arn when create_github_oidc_provider is false."
  }
}
