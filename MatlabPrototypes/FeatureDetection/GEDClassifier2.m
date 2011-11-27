function [score] = GEDClassifier2(x,ug,ubb,ubh,covarg,covarbb,covarbh)

      dGC = (x - ug) * (inv(covarg)) * transpose(x - ug);
      dGD = (x - ubb) * (inv(covarbb)) * transpose(x - ubb);
      dGE = (x - ubh) * (inv(covarbh)) * transpose(x - ubh);

      dC = dGC(1)^0.5;
      dD = dGD(1)^0.5;
      dE = dGE(1)^0.5;

      if (dC < dD) && (dC < dE)
        %label = 'G';
        score = 3;
      elseif (dD < dC) && (dD < dE)
        %label = 'BB';
        score = 2;
      elseif (dE < dC) && (dE < dD)
        %label = 'BH';
        score = 1;
      else
        %label = 'B';
        score = 0;
      end
end

