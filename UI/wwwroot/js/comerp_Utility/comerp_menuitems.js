import { SendRequest } from "./SendRequestUtility.js"

export const GetLoginUserMenuItems = async () => {
    const result = await SendRequest({ endpoint: '/Authorization/GetUserMenus' });
    if (result.success) {
        return JSON.parse(result.data) ;
    }
    return null;
}

export const getSubmenuActionListByName = async (submenuName) => {
    try {
        // Fetch the user menu items
        const result = await GetLoginUserMenuItems();

        if (!result || result.length === 0) {
            console.error('No menu data found.');
            return null;
        }

        // Iterate through each menu and its submenus
        for (const menu of result) {
            if (menu.SubMenus && menu.SubMenus.length > 0) {
                // Search in submenus for the provided submenuName
                const submenu = menu.SubMenus.find(sub => sub.SubMenuName === submenuName);
                if (submenu) {
                    // If the submenu is found, return the actions
                    return submenu.Actions ?? [];
                }
            }
        }

        // If no submenu found, return null or empty array
        console.warn(`No submenu found with name: ${submenuName}`);
        return null;

    } catch (error) {
        console.error('Error fetching submenu action list:', error);
        return null;
    }
};

// Fetch and get submenu related action list
export const getSubmenuActionListById = async (submenuId) => {
    try {
        // Fetch the user menu items
        const result = await GetLoginUserMenuItems();

        if (!result || result.length === 0) {
            console.error('No menu data found.');
            return null;
        }

        // Iterate through each menu and its submenus
        for (const menu of result) {
            if (menu.SubMenus && menu.SubMenus.length > 0) {
                // Search in submenus for the provided submenuId
                const submenu = menu.SubMenus.find(sub => sub.SubMenuId === submenuId);
                if (submenu) {
                    // If the submenu is found, return the actions
                    return submenu.Actions ?? [];
                }
            }
        }

        // If no submenu found, return null or empty array
        console.warn(`No submenu found with ID: ${submenuId}`);
        return null;

    } catch (error) {
        console.error('Error fetching submenu action list:', error);
        return null;
    }
};