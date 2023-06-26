<template>
    <div class="flex flex-col justify-center items-center mx-5 my-5">
        <!--
            https://ui.vuestic.dev/ui-elements/card
        -->

        <div class="grid gap-4 grid-cols-3">
            <!--
                <va-card
                    href="https://vuestic.epicmax.co/"
                    target="_blank"
                >
            -->
            <va-card
                color="primary"
                gradient
            >
                <va-card-title>Project Charter</va-card-title>
                <va-card-content>
                    Project Charter is a detailed document that outlines the goals, stakeholders, scope and other relevant information regarding a project.
                </va-card-content>
                <img src="@/assets/images/documents/project-charter.webp" class="h-54 bg-base-100" alt="Project charter" />
            </va-card>

            <va-card
                color="primary"
                gradient
                :bordered="false"
            >
                <va-card-title>App Requirements</va-card-title>
                <va-card-content>
                    App Requirements is a detailed document that outlines the user personas, features, and functionality of a software application.
                </va-card-content>
                <img src="@/assets/images/documents/app-req.webp" class="mt-2 h-54 bg-base-100" alt="" />
            </va-card>

            <va-card
                color="primary"
                gradient
            >
                <va-card-title>Feature Requirements</va-card-title>
                <va-card-content>
                    Contains everything the development team needs to start working on a new feature.
                </va-card-content>
                <img src="@/assets/images/documents/feature-req.webp" class="mt-2 h-54 bg-base-100" alt="" />
            </va-card>

            <va-card
                color="primary"
                gradient
            >
                <va-card-title>User story</va-card-title>
                <va-card-content>
                    User story descrciption
                </va-card-content>
                <va-image
                    src="@/assets/images/documents/feature-req.webp"
                    class="h-56"
                />
            </va-card>

            <!--TO DO CARD-->
            <va-card
                color="secondary"
                stripe
                stripe-color="success"
            >
                <va-card-title>Title</va-card-title>
                <va-card-content>
                    Lorem ipsum dolor sit amet, consectetur adipiscing elit.
                </va-card-content>
            </va-card>
        </div>
    </div>
</template>