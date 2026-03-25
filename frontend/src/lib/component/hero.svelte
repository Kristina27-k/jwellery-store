<script>
    import opacuityimg from "$lib/img/opacity.png";
    import heeroing from "$lib/img/bg.png";
    import earring from "$lib/img/earring.png";
    import Nav from "$lib/component/nav.svelte";
    // import "../app.css";
</script>

<section class="relative">
    <div class="h-100 w-full">
        <img class="h-full w-full object-cover" src={heeroing} alt="test" />
    </div>
    <div class="absolute h-60 w-full top-0">
        <img class="h-full w-full" src={opacuityimg} alt="opacity" />
    </div>
    <div class="absolute top-20 left-130">
        <h1 class="  uppercase text-[36px] ">the kristy store</h1>
    </div>
    <div class="absolute top-35 left-145">
<button class="bg-black h-10 w-40">
   <h1 class="z-999 text-white ">click here</h1>
</button>
    </div>
</section>
