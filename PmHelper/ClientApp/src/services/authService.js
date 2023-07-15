import Cookies from 'js-cookie';
import { COOKIE_NAME } from '@/config';

function useAuthService() {
    function isAuthenticated() {
        const cookie = Cookies.get(COOKIE_NAME);

        return Boolean(cookie);
    }

    return {
        isAuthenticated,
    };
}

export default useAuthService;