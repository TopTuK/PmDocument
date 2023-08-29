import axios from 'axios';
import { DEBUG  } from '@/config';

export default function useUserService() {
    const GET_ALL_USERS_ACTION = "user/GetAllUsers";

    async function getAllUsers() {
        console.log("UserService::getAllUsers: start getting all users");

        try {
            let response = await axios.get(GET_ALL_USERS_ACTION);

            // Check for bad request status
            if (response.status !== 200) {
                console.error('UserService::getAllUsers: http request error: ', response.status);
                return null;
            }
            else {
                const users = response.data;
                
                console.log('UserService::getAllUsers: return users: ', users.length);
                return users;
            }
        }
        catch (ex) {
            console.error('UserService::getAllUsers: exception raised', ex);
            return null;
        }
    }

    return {
        getAllUsers,
    };
}