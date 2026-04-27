<script lang="ts">
// import { jewelry } from '$lib/services/jewelryService';
import Fileupload from "$lib/components/common/fimageupload.svelte";

</script>

  <title>Update Product</title>
  <style>
    body {
      font-family: Arial, sans-serif;
      background: #f0f2f5;
      display: flex;
      justify-content: center;
      align-items: center;
      height: 100vh;
    }
    form {
      background: #fff;
      padding: 2rem;
      border-radius: 8px;
      box-shadow: 0 4px 12px rgba(0,0,0,0.1);
      width: 400px;
    }
    form h2 {
      text-align: center;
      margin-bottom: 1rem;
    }
    label {
      display: block;
      margin-top: 1rem;
      font-weight: bold;
    }
    input[type="text"],
    input[type="number"],
    textarea,
    select {
      width: 100%;
      padding: 0.5rem;
      margin-top: 0.3rem;
      border-radius: 4px;
      border: 1px solid #ccc;
    }
    button {
      margin-top: 1.5rem;
      width: 100%;
      padding: 0.7rem;
      background: #4CAF50;
      color: white;
      border: none;
      border-radius: 4px;
      font-size: 1rem;
      cursor: pointer;
    }
    button:hover {
      background: #45a049;
    }
  </style>


  <form action="/update-product" method="POST">
    <h2>Update Product</h2>

    <input type="hidden" name="id" value="<!-- Set product ID here -->">

    <label for="name">Product Name</label>
    <input type="text" id="name" name="name" placeholder="Enter product name" required>

    <label for="description">Description</label>
    <textarea id="description" name="description" rows="4" placeholder="Enter description" required></textarea>

    <label for="price">Price</label>
    <input type="number" id="price" name="price" step="0.01" placeholder="Enter price" required>

    
    <label for="category_id">Category</label>
    <select id="category_id" name="category_id" required>
      <option value="">-- Select Category --</option>
      <option value="1">Jewelry</option>
      <option value="2">Accessories</option>
      
    
    </select>
<Fileupload />
    <button type="submit">Update Product</button>
  </form>

