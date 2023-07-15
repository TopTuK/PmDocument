import axios from 'axios';
import { reactive } from 'vue';
import { DEBUG  } from '@/config';

export default function useUserService() {
    const GET_USER_INFO_ACTION = "user/getuserinfo";

    // Creating reactive object to hold user data and loading state
    const userState = reactive({
        userInfo: {},
        loading: false,
        error: false,
    });

    // Defining userService functions
    async function getUserInfo() {
        if (DEBUG) {
            console.log("UserService::getUserInfo: start getting user information");
        }

        try {
            userState.error = false;
            userState.loading = true; // Setting loading state to true

            const response = await axios.get(GET_USER_INFO_ACTION);

            if (DEBUG) {
                console.log(`UserService::getUserInfo: got response with status=${response.status}`);
            }

            // Check for bad request status
            if (response.status !== 200) {
                console.log("UserService::getUserInfo: ERROR: can't get user info");

                userState.error = true;
                userState.userInfo = {};
            }
            else {
                if (DEBUG) {
                    console.log("UserService::getUserInfo: userInfo=", response.data);
                }

                userState.userInfo = response.data; // Setting user info
            }

            userState.loading = false; // Setting loading state to false
        }
        catch (error) {
            console.log("UserService::getUserInfo: EXCEPTION: ", error);

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