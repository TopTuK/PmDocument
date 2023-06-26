import Cookies from 'js-cookie';

function useAuthService() {
    const appCookieName = "pmdoc_access";

    function isAuthenticated() {
        const cookie = Cookies.get(appCookieName);

        return Boolean(cookie);
    }

    return {
        isAuthenticated,
    };
}

export default useAuthService;