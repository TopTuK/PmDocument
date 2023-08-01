export const filter = function(text, length, clamp){
    clamp = clamp || '...';

    return text.length > length 
        ? text.slice(0, (length-3)) + clamp 
        : text;
};