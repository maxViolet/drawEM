# v4 / S4-03: acceptance criteria

**Task:** [render confetti and focus ring](task.md). **Plan:** [implementation](plan.md).

- [ ] Confetti and focus ring visibly start, animate, and leave no residual pixels or active frame subscription after their specified duration.
- [ ] The focus ring stays at its captured cursor point while the pointer moves within the monitor.
- [ ] Replacement and Stop clear old visuals; animation failures clear partial visuals and leave drawing and sound usable.
- [ ] Automated lifecycle checks and separate Windows visual checks using a development-only trigger are recorded; callback cost and visible pacing are measured against the roadmap targets. S4-06 replaces this trigger.

## Validation

- Automated checks: unverified. Record exact commands and results during implementation.
- Published-app Windows desktop checks: unverified. Record setup and observations separately from automated results.
