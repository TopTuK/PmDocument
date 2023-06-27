<template>
    <div class="flex flex-col justify-center items-center mx-5 my-5">
        <!--
            https://ui.vuestic.dev/ui-elements/card
        -->

        <div class="grid gap-4 lg:grid-cols-3 grid-cols-2">
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
                    <img 
                        src="@/assets/images/documents/project-charter.webp" 
                        class="h-48 bg-base-100"
                        alt="Project charter" 
                    />
                    <p class="mt-2">
                        Project Charter is a detailed document that outlines the goals, stakeholders, scope and other relevant information regarding a project.
                    </p>
                </va-card-content>
            </va-card>

            <va-card
                color="primary"
                gradient
                :bordered="false"
            >
                <va-card-title>App Requirements</va-card-title>
                <va-card-content>
                    <img
                        src="@/assets/images/documents/app-req.webp" 
                        class="h-48 bg-base-100"
                        alt=""
                    />
                    <p class="mt-2">
                        App Requirements is a detailed document that outlines the user personas, features, and functionality of a software application.
                    </p>
                </va-card-content>
                
            </va-card>

            <va-card
                color="primary"
                gradient
            >
                <va-card-title>Feature Requirements</va-card-title>
                <va-card-content>
                    <img
                        src="@/assets/images/documents/feature-req.webp"
                        class="object-cover h-48 bg-base-100"
                        alt=""
                    />
                    <p class="mt-2">
                        Contains everything the development team needs to start working on a new feature.
                    </p>
                </va-card-content>
            </va-card>

            <va-card
                color="primary"
                gradient
            >
                <va-card-title>User story</va-card-title>
                <va-card-content>
                    <img
                        src="@/assets/images/documents/user-story.png"
                        class="object-cover mt-2 h-48 bg-base-100"
                        alt=""
                    />
                    <p class="mt-2">
                        Create a well formed user story as a role with testable acceptance criteria.
                    </p>
                </va-card-content>
            </va-card>

            <!--TO DO CARD-->
            <va-card
                color="secondary"
                stripe
                stripe-color="success"
            >
                <va-card-title>Can't find the desired document?</va-card-title>
                <va-card-content>
                    <p>
                        Tell us which document you want to generate, and we will do our best to create it 👇
                    </p>
                    <div class="mt-6 flex flex-col items-center justify-center">
                        <va-button preset="primary">
                            Sign In
                        </va-button>
                    </div>
                </va-card-content>
            </va-card>
        </div>
    </div>
</template>