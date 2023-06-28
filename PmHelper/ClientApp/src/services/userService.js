import axios from 'axios';
import { reactive } from 'vue';

export default function useUserService() {
    // Creating reactive object to hold user data and loading state
    const userState = reactive({
        userInfo: {},
        loading: false,
        error: false,
    });

    // Defining userService functions
    async function getUserInfo() {
        try {
            userState.error = false;
            userState.loading = true; // Setting loading state to true

            const response = await axios.get('/user/getuserinfo');

            // Check for bad request status
            if (response.status !== 200) {
                userState.loading = false;
                userState.error = true;
            }

            userState.userInfo = response.data; // Setting user info
            userState.loading = false; // Setting loading state to false
        } catch (error) {
            userState.loading = false; // Setting loading state to false in case of error
            userState.error = true;
            throw error;
        }
    };

    return {
        userState,
        getUserInfo,
    };
}