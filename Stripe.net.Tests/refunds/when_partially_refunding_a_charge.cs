﻿using System.Collections.Generic;
using Machine.Specifications;

namespace Stripe.Tests
{
    public class when_partially_refunding_a_charge_with_refund_service
    {
        private static StripeRefund _stripeRefund;
        private static StripeRefundService _stripeRefundService;
        private static string _createdStripeChargeId;

        Establish context = () =>
        {
            _stripeRefundService = new StripeRefundService();

            var chargeService = new StripeChargeService();

            var stripeCharge = chargeService.Create(test_data.stripe_charge_create_options.ValidCard());
            _createdStripeChargeId = stripeCharge.Id;
        };

        Because of = () =>
            _stripeRefund = _stripeRefundService.Create(_createdStripeChargeId, new StripeRefundCreateOptions()
            {
                Amount = 300,
                Reason = StripeRefundReasons.RequestedByCustomer,
                Metadata = new Dictionary<string, string>() {{ "key", "value"}}
            });

        It should_create_with_stripe_charge_id = () =>
            _stripeRefund.ChargeId.ShouldEqual(_createdStripeChargeId);

        It should_refund_300 = () =>
            _stripeRefund.Amount.ShouldEqual(300);

        It should_have_reason = () =>
            _stripeRefund.Reason.ShouldEqual(StripeRefundReasons.RequestedByCustomer);

        It should_have_metadata = () =>
            _stripeRefund.Metadata["key"].ShouldEqual("value");
    }
}