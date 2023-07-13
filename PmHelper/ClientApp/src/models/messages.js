// Ready translated locale messages
// The structure of the locale message is the hierarchical object structure with each locale as the top property

const messages = {
    en: {
        common: {
            app_name: "PM Document",
            signin_button_title: "Sign In",
        },
        home: {
            title: "Generate documentation with help of AI",
            description: "Stop wasting time writing documentation from scratch.",
            slogan: "Let the AI help you!",
            app_description: "will help you to leverage the power of ChatGPT to save time and money"
        },
        documents: {
            new_document_title: "Can't find the desired document?",
            new_document_description: "Tell us which document you want to generate, and we will do our best to create it 👇",
            new_document_button_title: "Tell us more",
        },
        documents_types: {
            charter_title: "Project Charter",
            charter_description: "Project Charter is a detailed document that outlines the goals, stakeholders, scope and other relevant information regarding a project.",
            charter_detailed_description: "<p>Project Charter is a detailed document that outlines the goals, stakeholders, scope and other relevant information regarding a project.</p><p>It describes the project deliverables, what the team intends to achieve, and how they plan to achieve it. The document sets the expectations for the project, clarifies the project requirements, and serves as a reference for all stakeholders involved in the project. It is a critical document that guides the project from beginning to end and ensures that the project team stays on track.</p>",

            app_requirements_title: "Application Requirements",
            app_requirements_description: "Appliaction Requirements is a detailed document that outlines the user personas, features, and functionality of a software application.",
            app_requirements_detailed_description: "<p>App Requirements is a detailed document that outlines the user personas, features, and functionality of a software application. It is a comprehensive guide that outlines the purpose of the app, its intended audience, platform requirements, programming language, and other technical features. The application requirements document serves as a blueprint for the software development team, providing them with a clear understanding of the tasks they need to complete to deliver the final product. It helps ensure that all stakeholders are on the same page regarding the application's scope, objectives, and features.</p>",

            feature_requirements_title: "Feature Requirements",
            feature_requirements_description: "Feature requirements contain everything the development team needs to start working on a new feature.",
            feature_requirements_detailed_description: "<p>Project Charter is a detailed document that outlines the goals, stakeholders, scope and other relevant information regarding a project.</p>" +
                "<p>It describes the project deliverables, what the team intends to achieve, and how they plan to achieve it. The document sets the expectations for the project, clarifies the project requirements, and serves as a reference for all stakeholders involved in the project. It is a critical document that guides the project from beginning to end and ensures that the project team stays on track.</p>",
            
            user_story_title: "User Story",
            user_story_description: "Create a well formed user story as a role with testable acceptance criteria.",
            user_story_detailed_description: "",
        },
    }
};

export default messages;