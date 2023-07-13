<template>
    <div class="flex flex-col">
        
        <div class="container mx-auto pt-12">
            <div class="grid grid-cols-1 lg:grid-cols-2 gap-6 lg:gap-8">
                <article class="prose text-lg max-w-none">
                    <h1 class="text-4l md:text-5xl">
                        {{ $t('home.title') }}
                    </h1>

                    <div class="max-w-3xl">
                        <p>
                            {{ $t('home.description') }}
                            <b>{{ $t('home.slogan') }}</b>
                        </p>
                        <p>
                            <b>{{ $t('common.app_name') }}</b> {{ $t('home.app_description') }}
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

<script setup>
</script>

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