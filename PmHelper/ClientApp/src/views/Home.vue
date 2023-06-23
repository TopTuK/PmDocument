<template>
    <div class="flex flex-col">
        <!--<h1 class="text-3xl font-medium">HELLO WORLD!</h1>-->
        <div class="container mx-auto pt-12">
            <div class="grid grid-cols-1 lg:grid-cols-2 gap-6 lg:gap-8">
                <article class="prose text-lg max-w-none">
                    <h1 class="text-4l md:text-5xl">
                        Generate documentation with help of AI
                    </h1>

                    <div class="max-w-3xl">
                        <p>
                            Stop wasting time writing documentation from scratch.
                            <b>Let the AI help you!</b>
                        </p>
                        <p>
                            <b>PM Document</b> will help you to leverage the power of ChatGPT to save time and money
                        </p>
                    </div>
                </article>

                <div class="self-start order-first lg:order-none lg:pt-2">
                    <div class="mockup-window border shadow-xl sm:shadow-2xl border-base-300 bg-slate-900">
                        <img src="@/assets/images/project-charter.webp" class="bg-base-100" alt="Project charter" />
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>

<style scoped>
.mockup-window {
    border-radius: var(--rounded-box,1rem);
    overflow: hidden;
    overflow-x: auto;
    padding-top: 1.25rem;
    position: relative;
}

.mockup-window:before {
    border-radius: 9999px;
    box-shadow: 1.4em 0, 2.8em 0, 4.2em 0;
    content: "";
    padding-left: 1rem;
    color: white;
    display: block;
    height: 0.75rem;
    margin-bottom: 1rem;
    opacity: .3;
    width: 0.75rem;
}
</style>