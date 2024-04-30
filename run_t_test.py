import pandas as pd
import sys
import argparse
from scipy.stats import ttest_ind

def load_data(filename_1, filename_2):
    try:
        data1 = pd.read_csv(filename_1)
        data2 = pd.read_csv(filename_2)
        return data1, data2
    except FileNotFoundError as error:
        print(f"File not found: {error}")
        sys.exit(1)

def conduct_std_comparison(data1, data2):

    std_dev1 = data1['Value'].std()
    std_dev2 = data2['Value'].std()

    # Threshold for determining the point above which the difference is significant.
    significance_level = 0.05

    print(f"File 1 stand. dev.: {std_dev1}, File 1 stand. dev.: {std_dev2}")

    difference = abs(std_dev1 - std_dev2)

    if difference > significance_level:
        print("There is a significant difference")
    else:
        print("There is no significant difference")



if __name__ == "__main__":
    parser = argparse.ArgumentParser(description="T-Test Running Script.\nData files must have the .csv extension.")
    parser.add_argument("--file1", help="The filename of the first data file to process. Should end with .csv extension", required=True)
    parser.add_argument("--file2", help="The filename of the second data file to process. Should end with .csv extension", required=True)

    args = parser.parse_args()
    filename_1 = args.file1
    filename_2 = args.file2

    if not filename_1.endswith(".csv") or not filename_2.endswith(".csv"):
        print("Files must have the .csv extension!!!")
        sys.exit(1)

    data1, data2 = load_data(filename_1, filename_2)
    
    conduct_ttest(data1, data2)
