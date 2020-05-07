# -*- coding: utf-8 -*-

# Learn more: https://github.com/kennethreitz/setup.py

from setuptools import setup, find_packages

with open('readme.md') as f:
    readme = f.read()

setup(
    name='unium-tests',
    version='1.0.0',
    description='Examples and scaffolding for unium tests in python',
    long_description=readme,
    author='Gwaredd Mountain',
    author_email='gwaredd@hotmail.com',
    url='https://github.com/gwaredd/unium',
    license=license,
    packages=find_packages(exclude=('tests', 'docs'))
)
