CREATE TABLE IF NOT EXISTS payment_transactions (
    id SERIAL PRIMARY KEY,
    user_id INTEGER NOT NULL REFERENCES users(id) ON DELETE CASCADE,
    provider VARCHAR(20) NOT NULL,
    order_id VARCHAR(120) NOT NULL UNIQUE,
    provider_session_id VARCHAR(120) UNIQUE,
    provider_transaction_id VARCHAR(120),
    amount_paisa BIGINT NOT NULL,
    amount_rupees DECIMAL(18, 2) NOT NULL,
    status VARCHAR(30) NOT NULL DEFAULT 'Initiated',
    client_return_base_url TEXT NOT NULL,
    raw_response TEXT,
    created_at TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP,
    completed_at TIMESTAMP WITH TIME ZONE
);

CREATE INDEX IF NOT EXISTS idx_payment_transactions_user_id
    ON payment_transactions(user_id);

CREATE INDEX IF NOT EXISTS idx_payment_transactions_provider_status
    ON payment_transactions(provider, status);
