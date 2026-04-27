
INSERT INTO categories (cat_name, description) VALUES
('Rings', 'Beautiful rings for all occasions'),
('Necklaces', 'Elegant necklaces and pendants'),
('Bracelets', 'Stylish bracelets and bangles'),
('Earrings', 'Sparkling earrings and studs');

INSERT INTO jewelry_items (name, description, price, image_url, category_id) VALUES
('Diamond Solitaire Ring', 'A classic 1-carat diamond solitaire ring in 14k white gold.', 1200.00, 'https://example.com/images/diamond-ring.jpg', 1),
('Gold Chain Necklace', 'Simple and elegant 18k gold chain necklace.', 450.00, 'https://example.com/images/gold-necklace.jpg', 2),
('Silver Tennis Bracelet', 'Stunning silver bracelet with cubic zirconia stones.', 150.00, 'https://example.com/images/silver-bracelet.jpg', 3),
('Pearl Drop Earrings', 'Beautiful freshwater pearl drop earrings.', 80.00, 'https://example.com/images/pearl-earrings.jpg', 4),
('Sapphire Halo Ring', 'Gorgeous blue sapphire ring surrounded by a diamond halo.', 950.00, 'https://example.com/images/sapphire-ring.jpg', 1);

INSERT INTO users (username, email, password_hash, role) VALUES
('admin', 'admin@jewelrystore.com', '$2a$11$lQquJKwevp9R4A9vUJ5W.u9oMF9Oc.6sfqBgQ7UAfWzftjol.Njw6', 'Admin'),
('user1', 'user1@example.com', '$2a$11$lQquJKwevp9R4A9vUJ5W.u9oMF9Oc.6sfqBgQ7UAfWzftjol.Njw6', 'User');
