---
status: accepted
---

# GitHub releases may start unsigned; future signing depends on license and SignPath approval

drawEM keeps GitHub Releases as its distribution path and adds no paid signing
service under the current zero-cost constraint. The first public release may
therefore be unsigned; later SignPath Foundation signing is conditional on an
owner-selected qualifying open-source license, a release in the form to be
signed, and SignPath approval. This records the current boundary, not a choice
of license or a guarantee that SignPath will accept the project.

## Owner questions

1. Which OSI-approved license, if any, should apply to drawEM and all
   components? The repository currently has no detected license. SignPath's
   Foundation terms require an OSI-approved license without commercial
   dual-licensing for all components.
2. Is the owner willing to publish the source under that license and add the
   required license and code-signing policy to the repository and release page?
3. Is the owner willing to apply to SignPath Foundation and have
   `SignPath Foundation` appear as the certificate publisher? This also means
   enabling MFA, defining author/reviewer/approver roles, and manually
   approving every release signing request.
4. If SignPath declines or the owner does not choose an eligible license,
   should GitHub releases remain unsigned, or should the distribution/budget
   decision be reopened in a later ADR?

Until these questions are resolved, do not add paid signing services, change
the project license, or claim that future releases will be signed.

## Considered options

- **SignPath Foundation after qualification:** no service fee, but it requires
  an OSI-approved license, an existing release in the form to be signed,
  project approval, a published code-signing policy, MFA, and per-release
  manual approval. The certificate identifies SignPath Foundation as publisher.
  [Eligibility terms](https://signpath.org/terms.html)
- **Microsoft Artifact Signing or OV certificate/HSM:** rejected for now
  because these add direct costs. Microsoft's Basic tier is USD 9.99/account/
  month; OV signing also requires hardware-protected keys.
  [Microsoft pricing](https://learn.microsoft.com/en-us/azure/artifact-signing/how-to-change-sku)
- **Microsoft Store MSIX:** deferred because it changes the selected GitHub
  download path. Store signing does not sign the separate GitHub Setup.exe.
  [Microsoft distribution paths](https://learn.microsoft.com/en-us/windows/apps/package-and-deploy/choose-distribution-path)
