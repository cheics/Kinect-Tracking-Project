function p = c_colourMap(colour)

switch colour
    case 'green' 
        colour=[2,1,3];
    case 'red'
		colour=[1,2,3];
	case 'blue'
		colour=[3,2,1];
    otherwise
        warning('Unexpected colour');
end


m = size(get(gcf,'colormap'),1);
colourMat=hot(m);

p = sqrt((2*gray(m) + colourMat(:,colour))/3);
p=flipud(p);
