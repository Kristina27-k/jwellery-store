<script lang="ts">
    import { browser } from "$app/environment";
    import { goto } from "$app/navigation";
    import { onMount } from "svelte";

    let provider = "payment";
    let status = "failed";
    let providerStatus = "";
    let message = "We could not confirm the payment.";
    let orderId = "";
    let reference = "";

    onMount(() => {
        if (!browser) {
            return;
        }

        const params = new URLSearchParams(window.location.search);
        provider = params.get("provider") ?? "payment";
        status = params.get("status") ?? "failed";
        providerStatus = params.get("providerStatus") ?? "";
        message = params.get("message") ?? message;
        orderId = params.get("orderId") ?? "";
        reference = params.get("reference") ?? "";
    });

    $: providerLabel = provider === "esewa" ? "eSewa" : provider === "khalti" ? "Khalti" : "Payment";
    $: title =
        status === "success"
            ? `${providerLabel} payment complete`
            : status === "pending"
              ? `${providerLabel} payment pending`
              : `${providerLabel} payment not completed`;
</script>

<div class="status-page">
    <div class="status-card {status}">
        <div class="status-badge">{status.toUpperCase()}</div>
        <h1>{title}</h1>
        <p class="message">{message}</p>

        {#if providerStatus || orderId || reference}
            <div class="details">
                {#if providerStatus}
                    <div class="detail-row">
                        <span>Provider status</span>
                        <strong>{providerStatus}</strong>
                    </div>
                {/if}
                {#if orderId}
                    <div class="detail-row">
                        <span>Order ID</span>
                        <strong>{orderId}</strong>
                    </div>
                {/if}
                {#if reference}
                    <div class="detail-row">
                        <span>Reference</span>
                        <strong>{reference}</strong>
                    </div>
                {/if}
            </div>
        {/if}

        <div class="actions">
            <button class="btn-primary" on:click={() => goto("/")}>
                Continue shopping
            </button>
            <button class="btn-secondary" on:click={() => goto("/cart")}>
                Back to cart
            </button>
        </div>
    </div>
</div>

<style>
    .status-page {
        min-height: 100vh;
        display: grid;
        place-items: center;
        padding: 2rem;
        background:
            radial-gradient(circle at top, rgba(201, 162, 39, 0.18), transparent 28%),
            linear-gradient(180deg, #fffdf8 0%, #f5efe2 100%);
    }

    .status-card {
        width: min(100%, 560px);
        padding: 2.5rem;
        border-radius: 24px;
        background: rgba(255, 255, 255, 0.94);
        border: 1px solid rgba(201, 162, 39, 0.18);
        box-shadow: 0 24px 70px rgba(49, 36, 9, 0.12);
    }

    .status-card.pending {
        border-color: rgba(184, 134, 11, 0.28);
    }

    .status-card.failed {
        border-color: rgba(180, 35, 24, 0.18);
    }

    .status-badge {
        display: inline-flex;
        padding: 0.35rem 0.8rem;
        border-radius: 999px;
        background: #f8f4ea;
        color: #8b6a11;
        font-size: 0.8rem;
        font-weight: 700;
        letter-spacing: 0.08em;
    }

    h1 {
        margin: 1rem 0 0.75rem;
        font-size: clamp(2rem, 4vw, 2.8rem);
        color: #231b10;
    }

    .message {
        margin: 0;
        color: #5d5240;
        font-size: 1rem;
        line-height: 1.6;
    }

    .details {
        margin-top: 1.5rem;
        padding: 1.25rem;
        border-radius: 16px;
        background: #fbf8f2;
        display: grid;
        gap: 0.85rem;
    }

    .detail-row {
        display: flex;
        justify-content: space-between;
        gap: 1rem;
        color: #5d5240;
    }

    .detail-row strong {
        color: #231b10;
        text-align: right;
        word-break: break-word;
    }

    .actions {
        display: grid;
        gap: 0.75rem;
        margin-top: 1.75rem;
    }

    .btn-primary,
    .btn-secondary {
        width: 100%;
        padding: 1rem 1.2rem;
        border-radius: 12px;
        font-size: 1rem;
        font-weight: 600;
        cursor: pointer;
        transition: all 0.2s;
    }

    .btn-primary {
        border: none;
        background: #c9a227;
        color: white;
    }

    .btn-primary:hover {
        background: #b08d20;
        transform: translateY(-2px);
    }

    .btn-secondary {
        border: 1px solid #ddd2ba;
        background: transparent;
        color: #5d5240;
    }

    .btn-secondary:hover {
        background: #f7f1e4;
    }

    @media (max-width: 640px) {
        .status-card {
            padding: 1.75rem;
        }

        .detail-row {
            flex-direction: column;
        }

        .detail-row strong {
            text-align: left;
        }
    }
</style>
