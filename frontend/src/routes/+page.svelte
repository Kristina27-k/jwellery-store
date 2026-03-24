<!-- <script>
    let name = $state("dsf");
    const fullnames =[
        { first:'kristina' , last:'karki'},
        {first:'nishma' ,last:'shrestha'},
        {first:'barhsa' , last:'dhungana'},
    ]
    let fullname=[
        {first:'kriti', last:'karki'},
        {first:'kritika', last:'karki'},
        {first:'kreepa', last:'karki'},
    ]
//for map
  let uppercased = fullname.map(n => n.first.toUpperCase());
  let filtered = fullname.filter(n => n.first. startsWith('k'));
   let count = 0;
</script> -->

<!-- using input placeholder-->
<!-- {#each numbs as nume}
<h2>{nume}</h2>
{/each}
<h2></h2> -->

<!-- suing event handling using arrow method-->
<!-- <button on:click={() => (count = count +1 )}>count {count}</button> -->

<!--using map-->
<!-- {#each fullname as n}
<h1>{n.first} {n.last} {uppercased}</h1>
{/each} -->

<!--for binding-->
<!-- <h1>
    <input type="text" bind:value={name} />
    {name}
</h1> -->
<!-- 
{#each fullnames as names , index}
<h2>{index+1} - {names.first} {names.last}</h2>
{/each} -->

<!-- $state -->
<!-- <script>
	let count = $state(0);
</script>

<button onclick={() => count++}>
	clicks: {count}
</button> -->
<!-- <script>

	let state = $state({
		todos: [
			{
				done: false,
				text: "add more todos"
			}
		],
		newTodo: ""
	});

	function addTodo() {
		if (state.newTodo.trim() !== "") {
			state.todos.push({
				done: false,
				text: state.newTodo
			});
			state.newTodo = "";
		}
	}
</script>

<h2>Todo App</h2>

<input 
	type="text" 
	placeholder="Enter a task"
	bind:value={state.newTodo}
/>

<button onclick={addTodo}>Add</button>

<ul>
	{#each state.todos as todo}
	<li>
		<input type="checkbox" bind:checked={todo.done}>

		<span style="text-decoration: {todo.done ? 'line-through' : 'none'}">
			{todo.text}
		</span>
	</li>
	{/each}
</ul>

<style>
	h2{
		font-family: Arial;
	}

	input{
		padding:6px;
		margin:5px;
	}

	button{
		padding:6px 10px;
		background:green;
		color:white;
		border:none;
	}

	li{
		margin:6px 0;
	}
</style> -->

<!-- <script>

    let person = $state.raw({
	name: 'Heraclitus',
	age: 49
});

// this will have no effect
person.age += 1;

// this will work, because we're creating a new person
person = {
	name: 'Heraclitus',
	age: 50
};
</script>
<main>
    kkk
</main> -->

<!-- <script>
	let name = "christina"
</script>
<h1>hello {name}</h1> -->

<!-- <script>
  let count = 0;
</script>

<p>Count: {count}</p>

<button on:click={() => count++}>Click Me</button> -->

<!-- Header.svelte -->
<script>

	import NavBar from "$lib/component/nav.svelte";
	import heeroing from "$lib/img/bg.png";
	export let title = "The Kristi Shop";
	import Herosection from "$lib/component/hero.svelte";
	// import jwellery from "$lib/component/";
	let name = "kristina";
	let count = 0;
</script>

<Herosection />
<section class="bg-[#E5DED5] px-10 py-5">
<h3 class="capitalize text-[20px] ">this week </h3>
<h2 class="capitalize text-[28px]">popular product</h2>
</section>
<section>
	<div class="grid grid-cols-4">
<div class="bg-white">
<img src="">

</div>
	</div>
</section>
<!-- for nav -->

<!-- <header>
  <h1 class="text-[20px]">{title}</h1>
  <h3>{name}</h3>
</header> -->

<!-- <style>
  header {
    background: #f0f0f0;
    padding: 1rem;
    text-align: center;
	color: black;
  }
  /* body{
	background-color: aquamarine;

  } */
</style> -->
