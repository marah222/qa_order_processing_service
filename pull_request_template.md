---
name: Feature/Bug Fix Submission
about: Use this template for submitting your tested code to the 'develop' branch.
title: 'Fix: [Briefly describe the fix]'
labels: ''
assignees: ''
---

## 📝 Description

the PR includes 3 tests to test the proper rejection of orders with insufficient funds .
also a fix to a bug in the order processing logic to not withdraw from the account if there aren't sufficient funds
---

## ✅ My Task

the task was to ensure rejections happen correctly and for the right reasons.

---

## 🧪 How I Tested This

- [ ] Tested the Standard Customer, Insufficient Funds case
- [ ] Tested the Standard Customer, Rush Order, Insufficient Funds case
- [ ] Tested the Standard Customer, Zero Balance

---

## 📊 Code Coverage Screenshot

*After the CI pipeline passes, take a screenshot of the coverage comment on your PR and paste it here.*

**(Paste Screenshot Here)**